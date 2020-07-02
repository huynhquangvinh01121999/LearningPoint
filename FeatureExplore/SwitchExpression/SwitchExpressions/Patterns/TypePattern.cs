﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchExpressions.Patterns
{
    public class TypePattern<T> : IEvaluateExpression<T>
    {
        public string EvaluateExpression(T criteria) => criteria switch
        {
            Int32 value => $"Type {nameof(Int32)}, Value = {value}",
            Int64 value => $"Type {nameof(Int64)}, Value = {value}",
            string value => $"Type {nameof(String)}, Value = {value}",
            List<int> value when  value.Count < 5 => $"Type Small {nameof(List<int>)}, Value = {value}",
            List<int> value => $"Type Big {nameof(List<int>)}, Value = {value}",
            null => "Null Detected",
            _ => $"Type Unknown"
        };

    }
}
