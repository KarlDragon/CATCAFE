using System.Threading.Channels;

namespace BE.Infrastructure.Queue;
public interface IRequestQueue<T>
{
    ValueTask EnqueueAsync(T item, CancellationToken cancellationToken);
    ValueTask<T> DequeueAsync(CancellationToken cancellationToken);

    ChannelReader<T> Reader { get; }
}