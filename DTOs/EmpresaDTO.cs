namespace CarWash.DTOs
{
    public sealed class EmpresaDTO
    {
        public int Revision { get; set; }
        public string NombreComercial { get; set; } = "";
        public string RazonSocial { get; set; } = "";
        public string TipoPersona { get; set; } = "";
        public string Nit { get; set; } = "";
        public string DigitoVerificacion { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Municipio { get; set; } = "";
        public string Departamento { get; set; } = "";
        public string Pais { get; set; } = "Colombia";
        public string Telefono { get; set; } = "";
        public string Correo { get; set; } = "";
        public string ResponsabilidadesTributarias { get; set; } = "";
        public string? LogoBase64 { get; set; }
    }
}