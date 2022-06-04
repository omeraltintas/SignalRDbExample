using ChartServer.Hubs;
using ChartServer.Models;
using Microsoft.AspNetCore.SignalR;
using TableDependency.SqlClient;

namespace ChartServer.Subscription
{
    public interface IDatabaseSubscription
    {
        void Configure(string tableName);
    }
    public class DatabaseSubscription<T> : IDatabaseSubscription where T : class, new()
    {
        IConfiguration _configuration;
        SqlTableDependency<T> _tableDependency;
        IHubContext<SatisHub> _hubContext;
        public DatabaseSubscription(IConfiguration configuration, IHubContext<SatisHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;
        }

        public void Configure(string tableName)
        {
            _tableDependency = new SqlTableDependency<T>(_configuration.GetConnectionString("SQL"),tableName);
            _tableDependency.OnChanged +=async (o, e) =>
            {
               
                SatisDbContext context = new SatisDbContext();
                var data = (from personel in context.Personellers
                            join satis in context.Satislars
                            on personel.Id equals satis.Personelid
                            select new { personel, satis }).ToList();
                List<object> datas = new List<object>();
                var personelIsımleri= data.Select(d => d.personel.Adi).Distinct().ToList();

                personelIsımleri.ForEach(p =>
                {
                    datas.Add(new
                    {
                        name = p,
                        data = data.Where(s => s.personel.Adi == p).Select(s => s.satis.Fiyat).ToList()
                    });
                });
                await _hubContext.Clients.All.SendAsync("receiveMsg", datas);
            };
            _tableDependency.OnError += (o, e) =>
            {

            };
            _tableDependency.Start();
        }
        ~DatabaseSubscription()
        {
            _tableDependency.Stop();
        }
    }
}
