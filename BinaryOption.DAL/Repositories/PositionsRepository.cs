﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryOptions.DAL;
using BinaryOptions.DAL.Data;

namespace BinaryOptions.DAL
{
    public class PositionsRepository
    {
        private readonly DBContext m_context;

        public PositionsRepository(DBContext context)
        {
            m_context = context;
        }

        public Position GetPositionById(Guid positionId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Position> GetPositions()
        {
            throw new NotImplementedException();
        }
    }
}