namespace UsbDataReceiver.Extensions;

public static class Extensions
{
    public const int mVperAmp = 185;
    public const int ACSoffset = 2550;
    public static double Round(this double value, int digits)
    {
        if (value < 0) value = 0;
        return Math.Round(value, digits, MidpointRounding.AwayFromZero);
    }
    public static double CalculateToType(this double value, MeasurementType type )
    {
        switch (type)
        {
            case MeasurementType.Ampere:
                var mVoltage = value * 1000;
                var ampere = (mVoltage - ACSoffset)/mVperAmp;
                return ampere ;
            case MeasurementType.Voltage:
                return value;
            case MeasurementType.Temperature:
                //TODO: implement temperature calculation
                return value;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }  
}