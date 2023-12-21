using MongoDB.Driver;
using MyMusic.Core.Models;

namespace MyMusic.Data.MongoDB.Setting
{
    public interface IDatabaseSettings
    {
        IMongoCollection<Composer> Composers { get; }
    }
}
