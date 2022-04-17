public interface IDatabaseObject
{
    string KeyName { get; }

    void WriteData(string[] paramsLine);
}