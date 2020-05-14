using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeLi.Planets.Weather.DataAccess
{
    public abstract class BaseEntity
    {
        public ObjectId Id { get; set; }
    }
}
