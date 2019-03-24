﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NetManager.Domain.Dto {
	public class AdapterInfo {

		public AdapterInfo( string id, string name, string description ) {
			Id = id;
			Name = name;
			Description = description;
		}

		public string Id { get; }
		public string Name { get; }
		public string Description { get; }
	}
}