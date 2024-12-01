public static class string_pipe
{
    public static bool pipe_check(this string value, Func<string, bool> rule_to_check)
    {
        return rule_to_check(value);
    }
}