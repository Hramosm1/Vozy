namespace vozy_v2_api.models
{
    public class body
    {
        
        public dynamic content { get; set; }
    }
    public class Campaign
    {
        public Campaign(string? acuerdo, string? agent_name, string? campaign_id, int? codigo_banco, string? company, string? conoce_al_contacto, string? contact_id, string? contactabilidad, string? contesta_llamada, string? fecha, string? fecha_limite, string? fecha_limite_2, string? fecha_pagada, string? finalizada_de_llamada, string? hora, string? identificacion, string? llamar_de_nuevo, string? minutos, string? moneda, string? nombre, string? nombre_del_banco, float? pago_de_intencion, string? promesa_de_pago, string? promesa_de_pago_2, string? promesa_de_pago_horario, string? razon_de_no_pago, string? session_id, string? tel_llamada, string? telefono_1, string? telefono_2, string? telefono_3, string? telefono_acuerdo_de_pago_dado, float? valor_a_pagar)
        {
            this.acuerdo = acuerdo;
            this.agent_name = agent_name;
            this.campaign_id = campaign_id;
            this.codigo_banco = codigo_banco;
            this.company = company;
            this.conoce_al_contacto = conoce_al_contacto;
            this.contact_id = contact_id;
            this.contactabilidad = contactabilidad;
            this.contesta_llamada = contesta_llamada;
            this.fecha = fecha;
            this.fecha_limite = fecha_limite;
            this.fecha_limite_2 = fecha_limite_2;
            this.fecha_pagada = fecha_pagada;
            this.finalizada_de_llamada = finalizada_de_llamada;
            this.hora = hora;
            this.identificacion = identificacion;
            this.llamar_de_nuevo = llamar_de_nuevo;
            this.minutos = minutos;
            this.moneda = moneda;
            this.nombre = nombre;
            this.nombre_del_banco = nombre_del_banco;
            this.pago_de_intencion = pago_de_intencion;
            this.promesa_de_pago = promesa_de_pago;
            this.promesa_de_pago_2 = promesa_de_pago_2;
            this.promesa_de_pago_horario = promesa_de_pago_horario;
            this.razon_de_no_pago = razon_de_no_pago;
            this.session_id = session_id;
            this.tel_llamada = tel_llamada;
            this.telefono_1 = telefono_1;
            this.telefono_2 = telefono_2;
            this.telefono_3 = telefono_3;
            this.telefono_acuerdo_de_pago_dado = telefono_acuerdo_de_pago_dado;
            this.valor_a_pagar = valor_a_pagar;
        }
        
        public string? _id { get; set; }
        public string? acuerdo { get; set; }
        public string? agent_name { get; set; }
        public string? campaign_id { get; set; }
        public int? codigo_banco { get; set; }
        public string? company { get; set; }
        public string? conoce_al_contacto { get; set; }
        public string? contact_id { get; set; }
        public string? contactabilidad { get; set; }
        public string? contesta_llamada { get; set; }
        public string? fecha { get; set; }
        public string? fecha_limite { get; set; }
        public string? fecha_limite_2 { get; set; }
        public string? fecha_pagada { get; set; }
        public string? finalizada_de_llamada { get; set; }
        public string? hora { get; set; }
        public string? identificacion { get; set; }
        public string? llamar_de_nuevo { get; set; }
        public string? minutos { get; set; }
        public string? moneda { get; set; }
        public string? nombre { get; set; }
        public string? nombre_del_banco { get; set; }
        public float? pago_de_intencion { get; set; }
        public string? promesa_de_pago { get; set; }
        public string? promesa_de_pago_2 { get; set; }
        public string? promesa_de_pago_horario { get; set; }
        public string? razon_de_no_pago { get; set; }
        public string? session_id { get; set; }
        public string? tel_llamada { get; set; }
        public string? telefono_1 { get; set; }
        public string? telefono_2 { get; set; }
        public string? telefono_3 { get; set; }
        public string? telefono_acuerdo_de_pago_dado { get; set; }
        public float? valor_a_pagar { get; set; }

    }
}
