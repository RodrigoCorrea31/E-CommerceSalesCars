﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceSalesCars.Dominio.Interfaces
{
    public interface IRepositorioTransaccion
    {
        Task FinalizarTransaccionAsync(int transaccionId);
    }
}
