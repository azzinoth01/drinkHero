using System.Text.RegularExpressions;

public static class RegexPatterns {

    public static Regex GetValueMessage = new Regex("(?<=(?<!\\\\)VALUE)[\\s\\S]*?(?=(?<!\\\\)(END|NEXT))");
    public static Regex SplitValue = new Regex("(?<!\\\\);");
    public static Regex PropertyName = new Regex("[\\s\\S]*?(?=\")");
    public static Regex PropertyValue = new Regex("(?<=\")[\\s\\S]*(?=\")");
    public static Regex ObjectCommand = new Regex("OBJECT (VALUE|COMMAND)");
    public static Regex GetCompleteMessage = new Regex("OBJECT(?<=(?<!\\\\)OBJECT)[\\s\\S]*?(?=(?<!\\\\)(END))END");
    public static Regex GetCallFunctionName = new Regex("(?<=(?<!\\\\)COMMAND)[\\s\\S]*?(?=(?<!\\\\)(PARAMETER|END))");
    public static Regex GetCallFunctionParameter = new Regex("(?<=(?<!\\\\)PARAMETER)[\\s\\S]*?(?=(?<!\\\\)(END))");
    public static Regex CheckDataIsEmpty = new Regex("(?=(?<!\\\\)(END))END");
    public static Regex CheckKeepAlive = new Regex("(?=(?<!\\\\)(KEEPALIVE))KEEPALIVE");


}
