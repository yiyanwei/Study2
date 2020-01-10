using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ZeroOne.Extension
{
    /// <summary>
    /// methodof运算重载
    /// </summary>
    /// <typeparam name="T">必须是委托类型</typeparam>
    public class methodof<T> where T : Delegate
    {
        private MethodInfo method;

        public methodof(T func)
        {
            Delegate del = (Delegate)(object)func;
            this.method = del.Method;
        }

        public static implicit operator methodof<T>(T methodof)
        {
            return new methodof<T>(methodof);
        }

        public static implicit operator MethodInfo(methodof<T> methodof)
        {
            return methodof.method;
        }
    }
    //MethodInfo writeln = (methodof<Action>)Console.WriteLine;
    //MethodInfo parse = (methodof<Func<string, int>>)int.Parse;
}
