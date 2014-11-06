namespace WpfTools4.Commands
{
    /// <summary>
    /// Encapsulates a method that takes two parameter and does not return a value.
    /// </summary>
    /// <typeparam name="T1">The 1. type of the parameter of the method that this delegate encapsulates</typeparam>
    /// <typeparam name="T2">The 2. type of the parameter of the method that this delegate encapsulates</typeparam>
    /// <param name="obj1">The 1. parameter of the method that this delegate encapsulates</param>
    /// <param name="obj2">The 2. parameter of the method that this delegate encapsulates</param>
    public delegate void Action<T1, T2>(T1 obj1, T2 obj2);
}
