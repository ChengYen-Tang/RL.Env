﻿namespace RL.Env;

/// <summary>
/// Error superclass.
/// </summary>
public class Error : Exception
{
    public Error(string message) : base(message) { }
}
