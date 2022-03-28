namespace WebApiArtistas.Services
{
    public class ProfeGod : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string archivopro = "MiProfewFav.txt";
        private Timer timer;

        public ProfeGod(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(120));
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            return Task.CompletedTask;
        }
        private void DoWork(object state)
        {
            Escribir("El Profe Gustavo Rodriguez es el mejor");
        }
        private void Escribir(string msg)
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{archivopro}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true)) { writer.WriteLine(msg); }
        }
    }
}
