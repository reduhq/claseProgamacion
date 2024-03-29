﻿using AppCore.IServices;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Services
{
    public class ActivoServices:BaseServices<Activo>,IActivoServices // ActivoServices as IActivoService
    {
        IActivoModel activoModel; // IActivoModel is BinaryActivoRepository
        public ActivoServices(IActivoModel model) : base(model)
        {
            this.activoModel = model;
        }

        public bool Delete(int id)
        {
            return activoModel.Delete(id);
        }

        public Activo GetById(int id)
        {
            return activoModel.GetById(id);
        }

        public void Update(Activo t)
        {
            activoModel.Update(t);
        }
    }

}
