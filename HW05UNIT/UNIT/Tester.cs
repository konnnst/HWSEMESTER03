using System.Diagnostics;

namespace Unit;

/// <summary>
/// Runs test for given assembly
/// </summary>
public class Tester
{
    private Assembly _asm;

    /// <summary>
    /// Creates tester object using path
    /// </summary>
    /// <param name="asm">Assembly path</param>
    public Tester(Assembly asm)
    {
        _asm = asm;
    }

    private bool IsSkipTestMethod(Action testMethod)
    {
        var ignore = (testMethod.Method.GetCustomAttribute(typeof(Test)) as Test).Ignore;
        return ignore != "NORMAL";
    } 
    private string GetSkipMessage(Action testMethod)
    {
        return (testMethod.Method.GetCustomAttribute(typeof(Test)) as Test).Ignore;
    }
    private bool ValidateMethod(MethodInfo method, IEnumerable<Attribute> attrs)
    {
        var flag = false;

        foreach (var attr in attrs)
        {
            if (attr is Test || attr is BeforeClass || attr is AfterClass || attr is Before || attr is After)
            {
                flag = true;
                break;
            }
        }

        if (!flag)
        {
                return false;
        }

        if (method.GetParameters().Count() != 0 || method.ReturnType.Name != "Void" || method.IsPublic != true)
        {
            throw new Exception($"ACHTUNG! Test method {method.Name} doesn't match \"public void Method()\" prototype");
        }

        return true;
    }
    private Action GetSpecialMethod(MethodInfo method)
    {
        return (Action)Delegate.CreateDelegate(typeof(Action), null, method);
    }
    private void GetSpecialMethods(Type t, Action? beforeClass, Action? afterClass,
                                    Action? before, Action? after, List<Action> testMethods)
    {
        foreach (var method in t.GetMethods())
        {
            var attrs = method.GetCustomAttributes();

            if (ValidateMethod(method, attrs))
            {
                var testMethod = GetSpecialMethod(method);

                foreach (var attr in attrs)
                {

                    if (attr is Test)
                    {
                        testMethods.Add(testMethod);
                    }
                    if (attr is BeforeClass)
                    {
                        if (beforeClass != null)
                        {
                            throw new Exception("ACHTUNG! There can't be more than one beforeClass method for each class");
                        }
                        beforeClass = testMethod;
                    }
                    if (attr is AfterClass)
                    {
                        if (afterClass != null)
                        {
                            throw new Exception("ACHTUNG! There can't be more than one afterClass method for each class");
                        }
                        afterClass = testMethod;
                    }
                    if (attr is Before)
                    {
                        if (before != null)
                        {
                            throw new Exception("ACHTUNG! There can't be more than one before method for each class");
                        }
                        beforeClass = testMethod;
                    }
                    if (attr is AfterClass)
                    {
                        if (after != null)
                        {
                            throw new Exception("ACHTUNG! There can't be more than one after method for each class");
                        }
                        after = testMethod;
                    }
                }
            }
        }
    }

    private bool IsExpectedException(Exception ex, Action testMethod)
    {
        var expected_list = (testMethod.Method.GetCustomAttribute(typeof(Test)) as Test).Expected;
        return expected_list.Contains(ex.Message);
    }
    private void RunSpecialMethods(TestClassInfo testClassInfo, Action? beforeClass, Action? afterClass,
                                    Action? before, Action? after, List<Action> testMethods)
    {
        var stopwatch = new Stopwatch();

        if (beforeClass != null)
        {
            beforeClass();
        }
        foreach (var testMethod in testMethods)
        {
            if (IsSkipTestMethod(testMethod))
            {
                var skipMessage = GetSkipMessage(testMethod);
                var skippedTestMethod = new SkippedTestMethodInfo(testMethod.Method.Name, 0, skipMessage);
                testClassInfo.AddSkipped(skippedTestMethod);
            }
            else
            {
                if (before != null)
                {
                    before();
                }

                var exceptionFlag = false;
                try
                {
                    stopwatch.Start();
                    testMethod();
                    stopwatch.Stop();
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();
                    if (!IsExpectedException(ex, testMethod))
                    {

                        var exceptionMessage = ex.Message;
                        exceptionFlag = true;
                        var failedTestMethod = new FailedTestMethodInfo(testMethod.Method.Name,
                                                                    stopwatch.ElapsedMilliseconds,
                                                                    exceptionMessage);
                        testClassInfo.AddFailed(failedTestMethod);
                    }
                }

                if (!exceptionFlag)
                {
                    var correctTestMethod = new PassedTestMethodInfo(testMethod.Method.Name, stopwatch.ElapsedMilliseconds);
                    testClassInfo.AddPassed(correctTestMethod);
                }


                stopwatch.Reset();

                if (after != null)
                {
                    after();
                }
            }
        }
        if (afterClass != null)
        {
            afterClass();
        }
    }

    private bool IsTestClass(Type t)
    {
        foreach (var attr in t.GetCustomAttributes())
        {
            if (attr is UnitTestClass)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Runs test methods and logs results in selected assembly 
    /// </summary>
    public BuildInfo RunTests()
    {
        var build = new BuildInfo(_asm);
        foreach (var t in _asm.ExportedTypes)
        {
            if (!t.IsClass)
            {
                continue;
            }
            if (!IsTestClass(t))
            {
                continue;
            }

            var testClassInfo = new TestClassInfo(t);

            Action? beforeClass = null;
            Action? afterClass = null;
            Action? before = null;
            Action? after = null;
            List<Action> testMethods = new();
            GetSpecialMethods(t, beforeClass, afterClass, before, after, testMethods);
            InfoWriter.WriteStatsSpecialMethods(testClassInfo, beforeClass, afterClass, before, after, testMethods);
            RunSpecialMethods(testClassInfo, beforeClass, afterClass, before, after, testMethods);

            build.AddClassInfo(testClassInfo);
        }

        return build;
    }

}