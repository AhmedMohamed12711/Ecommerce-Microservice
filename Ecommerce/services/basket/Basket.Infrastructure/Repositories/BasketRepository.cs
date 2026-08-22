using Basket.Core.Entites;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories;

public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;
    private readonly ILogger<BasketRepository> _logger;

    public BasketRepository(
        IDistributedCache redisCache,
        ILogger<BasketRepository> logger)
    {
        _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        try
        {
            var basket = await _redisCache.GetStringAsync(userName);

            if (string.IsNullOrWhiteSpace(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex,
                "Failed to deserialize basket for user {UserName}",
                userName);

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error while getting basket for user {UserName}",
                userName);

            throw;
        }
    }

    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        ArgumentNullException.ThrowIfNull(shoppingCart);

        try
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };

            var json = JsonConvert.SerializeObject(shoppingCart);

            await _redisCache.SetStringAsync(
                shoppingCart.UserName,
                json,
                options);

            _logger.LogInformation(
                "Basket updated successfully for user {UserName}",
                shoppingCart.UserName);

            return shoppingCart;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex,
                "Serialization failed while updating basket for user {UserName}",
                shoppingCart.UserName);

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error while updating basket for user {UserName}",
                shoppingCart.UserName);

            throw;
        }
    }

    public async Task DeleteBasket(string userName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userName);

        try
        {
            await _redisCache.RemoveAsync(userName);

            _logger.LogInformation(
                "Basket deleted successfully for user {UserName}",
                userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error while deleting basket for user {UserName}",
                userName);

            throw;
        }
    }
}