namespace UsbDataReceiver.Extensions;

public static class Extensions
{
    public static double Round(this double value, int digits)
    {
        if (value < 0) value = 0;
        return Math.Round(value, digits, MidpointRounding.AwayFromZero);
    }
}