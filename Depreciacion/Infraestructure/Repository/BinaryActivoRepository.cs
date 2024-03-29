﻿using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class BinaryActivoRepository : IActivoModel
    {
        private RAFContext context;
        private const int SIZE = 119;
        public BinaryActivoRepository()
        {
            context = new RAFContext("activo", SIZE);
        }
        public int Add(Activo t)
        {
            try
            {
                return context.Create<Activo>(t);
            }
            catch (IOException)
            {
                throw;
            }
        }

        public void Delete(Activo t)
        {
            throw new NotImplementedException();
        }

        public Activo GetById(int id)
        {
            try
            {
                return context.Get<Activo>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Activo> Read()
        {
            try
            {
                return context.GetAll<Activo>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool Delete(int id)
        {
           return context.Delete(id);
        }

        public void Update(Activo t)
        {
            context.Update(t);
        }
    }
}
