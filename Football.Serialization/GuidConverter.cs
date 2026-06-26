namespace Football.Serialization
{
    public static class GuidConverter
    {
        public static Guid ConvertFromInt(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static int ConvertToInt(Guid guid)
        {
            byte[] bytes = guid.ToByteArray();
            return BitConverter.ToInt32(bytes, 0);
        }
    }
}
