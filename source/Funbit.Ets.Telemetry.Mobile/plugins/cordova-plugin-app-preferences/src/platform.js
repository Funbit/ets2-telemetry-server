function AppPreferencesLocalStorage() {
	this.watchChanges = false;
}

AppPreferencesLocalStorage.prototype.nativeFetch = function(successCallback, errorCallback, args) {

	var self = this;

	var key = args.key;

	if (args.dict)
		key = args.dict + '.' + args.key;

	var result = window.localStorage.getItem (key);

	var value = result;
	try {
		value = JSON.parse (result);
	} catch (e) {
	}
	successCallback (value);
};

AppPreferencesLocalStorage.prototype.nativeRemove = function(successCallback, errorCallback, args) {

	var self = this;

	var key = args.key;

	if (args.dict)
		key = args.dict + '.' + args.key;

	var result = window.localStorage.removeItem (key);

	if (typeof cordova !== "undefined" && this.watchChanges) {
		// https://w3c.github.io/webstorage/#the-storage-event

		// If the event is being fired due to an invocation of the setItem() or removeItem() methods,
		// the event must have its key attribute initialised to the name of the key in question,
		// its oldValue attribute initialised to the old value of the key in question,
		// or null if the key is newly added, and its newValue attribute initialised
		// to the new value of the key in question, or null if the key was removed.

		// Otherwise, if the event is being fired due to an invocation of the clear() method,
		// the event must have its key, oldValue, and newValue attributes initialised to null.

		// In addition, the event must have its url attribute initialised
		// to the address of the document whose Storage object was affected;
		// and its storageArea attribute initialised to the Storage object from the Window object
		// of the target Document that represents the same kind of Storage area as was affected
		// (i.e. session or local).

		cordova.fireDocumentEvent('preferencesChanged', {'key': args.key, 'dict': args.dict});
	}

	successCallback (true);
};

AppPreferencesLocalStorage.prototype.nativeStore = function(successCallback, errorCallback, args) {

	var self = this;

	var key = args.key;

	if (args.dict)
		key = args.dict + '.' + args.key;

	var value = JSON.stringify (args.value);

	window.localStorage.setItem (key, value);

	if (typeof cordova !== "undefined" && this.watchChanges) {
		cordova.fireDocumentEvent('preferencesChanged', {'key': args.key, 'dict': args.dict});
	}

	successCallback ();
};

AppPreferencesLocalStorage.prototype.clearAll = function (successCallback, errorCallback) {

	var self = this;

	window.localStorage.clear ();

	if (typeof cordova !== "undefined" && this.watchChanges) {
		cordova.fireDocumentEvent('preferencesChanged', {'key': null, 'dict': null, all: true});
	}

	successCallback ();
};

AppPreferencesLocalStorage.prototype.show = function (successCallback, errorCallback) {

	var self = this;

	errorCallback ('not implemented');
};

AppPreferencesLocalStorage.prototype.watch = function (successCallback, errorCallback, watchChanges) {
	// http://dev.w3.org/html5/webstorage/#localStorageEvent
	// http://stackoverflow.com/questions/4671852/how-to-bind-to-localstorage-change-event-using-jquery-for-all-browsers
	// When the setItem(), removeItem(), and clear() methods are called on a Storage object x
	// that is associated with a local storage area, if the methods did something,
	// then in every Document object whose Window object's localStorage attribute's
	// Storage object is associated with the same storage area, other than x,
	// a storage event must be fired

	// In other words, a storage event is fired on every window/tab except for the one
	// that updated the localStorage object and caused the event.

	// Firefox seems to have corrected the problem, however IE9 and IE10 still do not follow the spec.
	// They fire the event in ALL windows/tabs, including the one that originated the change.
	// I've raised this bug with Microsoft here (requires login unfortunately):
	// connect.microsoft.com/IE/feedback/details/774798/… –  Dave Lockhart Dec 19 '12 at 22:53

	// Doing <unbind, setItem, bind> as a workaround seems to work. –  estecb Dec 31 '13 at 8:38

	this.watchChanges = watchChanges === undefined ? true : watchChanges;
}

if (typeof module !== "undefined") {
	module.exports = new AppPreferencesLocalStorage();
}
