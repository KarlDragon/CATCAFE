namespace BE.Infrastructure.Queue;
public interface IRequestQueue<T>
{
    ValueTask EnqueueAsync(T item);
    ValueTask<T> DequeueAsync(CancellationToken cancellationToken);
}