namespace Moonlight.App.Helpers;

public class ProgressStream : Stream
{
    public Func<int, Task>? Progress { get; set; }
    private int LastPercent = -1;

    Stream InnerStream { get; init; }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var result = InnerStream.ReadAsync(buffer, offset, count).Result;

        int percentComplete = (int)Math.Round((double)(100 * Position) / Length);

        if (LastPercent == -1 || percentComplete - LastPercent > 5)
        {
            Task.Run(async () =>
            {
                if (Progress != null)
                    await Progress.Invoke(percentComplete);
            });
            
            LastPercent = percentComplete;
        }

        return result;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        InnerStream.WriteAsync(buffer, offset, count);
    }

    public override bool CanRead => InnerStream.CanRead;
    public override bool CanSeek => InnerStream.CanSeek;
    public override bool CanWrite => InnerStream.CanWrite;
    public override long Length => InnerStream.Length;

    public override long Position
    {
        get => InnerStream.Position;
        set => InnerStream.Position = value;
    }

    public ProgressStream(Stream s)
    {
        InnerStream = s;
    }

    public override void Flush() => InnerStream.Flush();
    public override long Seek(long offset, SeekOrigin origin) => InnerStream.Seek(offset, origin);
    public override void SetLength(long value) => InnerStream.SetLength(value);
}