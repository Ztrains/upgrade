using System;
using Java.Lang;

namespace UpgradeApp {
	public class chatClass {
		public bool direction; //True for left false for right
		public string message;

		public chatClass(bool direction, string message) {
			this.direction = direction;
			this.message = message;
		}

		public static explicit operator chatClass(Java.Lang.Object v) {
			throw new NotImplementedException();
		}
	}
}
