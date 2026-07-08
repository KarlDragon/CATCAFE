namespace BE.Infrastructure.Queue;
using System.Threading.Channels;
public class RequestQueue<T> : IRequestQueue<T>
{
    private readonly Channel<T> _channel;

    public RequestQueue(int capacity)
    {
        var options = new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait
        };
        _channel = Channel.CreateBounded<T>(options);
    }

    public async ValueTask EnqueueAsync(T item, CancellationToken cancellationToken)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item), "Cannot enqueue a null item.");
        }
        await _channel.Writer.WriteAsync(item, cancellationToken);
    }

    public async ValueTask<T> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _channel.Reader.ReadAsync(cancellationToken);
    }

    public ChannelReader<T> Reader => _channel.Reader;
}