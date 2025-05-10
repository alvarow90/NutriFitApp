using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using System.Net.Http.Json;


namespace NutriFitApp.WebAdmin.Helpers
{
    public static class RolHelper
    {
        public const string Administrador = "Administrador";
        public const string Nutriologo = "Nutriologo";
        public const string Entrenador = "Entrenador";
        public const string Usuario = "Usuario";

        public static bool EsAdmin(string? rol)
        {
            return rol == Administrador;
        }

        public static bool EsEspecialista(string? rol)
        {
            return rol == Nutriologo || rol == Entrenador;
        }
    }
}
