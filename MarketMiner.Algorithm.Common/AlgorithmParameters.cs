using MarketMiner.Api.Client.Common.Contracts;
using MarketMiner.Client.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketMiner.Algorithm.Common
{
   public class AlgorithmParameters : IParameters
   {
      IEnumerable<AlgorithmParameter> _parameters;

      public AlgorithmParameters(IEnumerable<AlgorithmParameter> parameters)
      {
         _parameters = parameters;
      }

      AlgorithmParameter _parameter;

      public string GetString(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "string" && p.ParameterName.ToLower() == name.ToLower());
         return _parameter.ParameterValue;
      }
      public bool? GetBoolean(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "boolean" && p.ParameterName.ToLower() == name.ToLower());
         try { return Convert.ToBoolean(_parameter.ParameterValue); }
         catch { return null; }
      }
      public double? GetDouble(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "double" && p.ParameterName.ToLower() == name.ToLower());
         try { return Convert.ToDouble(_parameter.ParameterValue); }
         catch { return null; }
      }
      public decimal? GetDecimal(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "decimal" && p.ParameterName.ToLower() == name.ToLower());
         try { return Convert.ToDecimal(_parameter.ParameterValue); }
         catch { return null; }
      }
      public short? GetShort(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "short" && p.ParameterName.ToLower() == name.ToLower());
         try { return Convert.ToInt16(_parameter.ParameterValue); }
         catch { return null; }
      }
      public int? GetInteger(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "integer" && p.ParameterName.ToLower() == name.ToLower());
         try { return Convert.ToInt32(_parameter.ParameterValue); }
         catch { return null; }
      }
      public long? GetLong(string name)
      {
         _parameter = _parameters.FirstOrDefault(p => p.ParameterType.ToLower() == "long" && p.ParameterName.ToLower() == name.ToLower());
         try { return Convert.ToInt64(_parameter.ParameterValue); }
         catch { return null; }
      }
   }
}
