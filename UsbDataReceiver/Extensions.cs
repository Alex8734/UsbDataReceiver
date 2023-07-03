namespace UsbDataReceiver.Extensions;

public static class Extensions
{
    public static double Round(this double value, int digits)
    {
        return Math.Round(value, digits, MidpointRounding.AwayFromZero);
    }
}