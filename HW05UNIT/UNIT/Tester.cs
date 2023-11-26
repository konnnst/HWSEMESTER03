using System.Diagnostics;

namespace Unit;
public class Tester
{
    private delegate void Action();
    private Assembly _asm;

    public Tester(Assembly asm)
    {
        _asm = asm;
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

    private void WriteStatsSpecialMethods(Type t, Action? beforeClass, Action? afterClass,
                                    Action? before, Action? after, List<Action> testMethods)
    {
        Console.WriteLine("Class {0}: " +
                        "test methods - {1}, " +
                        "beforeClass - {2}, " +
                        "afterClass - {3}, " +
                        "before - {4}, " +
                        "after - {5}", t.Name, testMethods.Count, Convert.ToInt32(beforeClass != null),
                        Convert.ToInt32(afterClass != null), Convert.ToInt32(before != null),
                        Convert.ToInt32(after != null));
    }

    private void RunSpecialMethods(Action? beforeClass, Action? afterClass,
                                    Action? before, Action? after, List<Action> testMethods)
    {
        var stopwatch = new Stopwatch();
        var elapsedTime = new int[testMethods.Count()];
        var failedCount = 0;
        var skippedCount = 0;

        Console.WriteLine("Starting tests... ");
        if (beforeClass != null)
        {
            beforeClass();
        }
        foreach (var testMethod in testMethods)
        {
            if (before != null)
            {
                before();
            }

            stopwatch.Start();
            try
            {
                testMethod();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error message in {testMethod.Method}: \n\t{ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
                failedCount++;
            }
            stopwatch.Stop();
            var time = stopwatch.ElapsedMilliseconds;
            stopwatch.Reset();

            if (after != null)
            {
                after();
            }
        }
        if (afterClass != null)
        {
            afterClass();
        }

        if (failedCount != 0)
        {
            Console.ForegroundColor = ConsoleColor.Red;
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
        }
        Console.WriteLine($"Tests finished: " +
                        $"passed - {testMethods.Count() - failedCount - skippedCount}, " +
                        $"failed - {failedCount}, " +
                        $"skipped - {skippedCount}, " +
                        $"total time elapsed - {elapsedTime.Sum()} ms\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    public void RunTests()
    {
        foreach (var t in _asm.ExportedTypes)
        {
            if (!t.IsClass)
            {
                continue;
            }

            Action? beforeClass = null;
            Action? afterClass = null;
            Action? before = null;
            Action? after = null;
            List<Action> testMethods = new();
            GetSpecialMethods(t, beforeClass, afterClass, before, after, testMethods);
            WriteStatsSpecialMethods(t, beforeClass, afterClass, before, after, testMethods);
            RunSpecialMethods(beforeClass, afterClass, before, after, testMethods);
            }
    }

}