﻿using System;
using Java.Lang;

namespace UpgradeApp {
	// A Helper class for chat objects in messaging and message board activities
	public class chatClass {
		public bool direction; // True for left false for right
		public string message; // The actual text

		public chatClass(bool direction, string message) {
			this.direction = direction;
			this.message = message;
		}

		public static explicit operator chatClass(Java.Lang.Object v) {
			throw new NotImplementedException();
		}
	}
}
