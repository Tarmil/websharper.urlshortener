namespace UrlShortener

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open WebSharper.AspNetCore

type Startup() =

    member this.ConfigureServices(services: IServiceCollection) =
        services.AddSitelet<Site>()
                .AddTransient<Database.Context>()
                .AddAuthentication("WebSharper")
                .AddCookie("WebSharper", fun options -> ())
        |> ignore

    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment, db: Database.Context) =
        if env.IsDevelopment() then app.UseDeveloperExceptionPage() |> ignore

        db.Migrate()

        app.UseAuthentication()
            .UseStaticFiles()
            .UseWebSharper()
            .Run(fun context ->
                context.Response.StatusCode <- 404
                context.Response.WriteAsync("Page not found"))

module Program =

    [<EntryPoint>]
    let main args =
        WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build()
            .Run()
        0
