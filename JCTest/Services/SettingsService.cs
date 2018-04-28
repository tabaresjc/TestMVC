using JCTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace JCTest.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly NameValueCollection settings;

        public SettingsService(NameValueCollection settings)
        {
            this.settings = settings;
        }

        public string Get(string name, string defaultValue = null)
        {
            var val = this.settings.Get(name);

            if (string.IsNullOrWhiteSpace(val))
            {
                return defaultValue;
            }

            return val;
        }

        public bool GetBoolean(string name, bool defaultValue = false)
        {
            bool value = false;

            if (!bool.TryParse(this.settings.Get(name), out value))
            {
                return defaultValue;
            }

            return value;
        }

        public int GetNumeric(string name, int defaultValue = 0)
        {
            int value = 0;

            if (!int.TryParse(this.settings.Get(name), out value))
            {
                return defaultValue;
            }

            return value;
        }
    }
}