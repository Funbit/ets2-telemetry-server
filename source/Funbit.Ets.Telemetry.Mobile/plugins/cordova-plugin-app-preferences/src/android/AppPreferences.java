package me.apla.cordova;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.CordovaWebView;
import org.apache.cordova.PluginResult;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;
import org.json.JSONTokener;

import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.content.SharedPreferences.OnSharedPreferenceChangeListener;
import android.preference.PreferenceManager;
import android.util.Log;

public class AppPreferences extends CordovaPlugin implements OnSharedPreferenceChangeListener {

	//    private static final String LOG_TAG = "AppPreferences";
	//    private static final int NO_PROPERTY = 0;
	//    private static final int NO_PREFERENCE_ACTIVITY = 1;
	private static final int COMMIT_FAILED = 2;
	private static final int NULL_VALUE = 3;
	private static CordovaWebView cdvWebView;
	private static boolean watchChanges = false;

	@Override
	protected void pluginInitialize() {
		cdvWebView = this.webView;
	}

	public void onSharedPreferenceChanged (SharedPreferences sharedPreferences, final String key) {
		Log.d("", "PREFERENCE CHANGE DETECTED FOR " + key);
		//cordova.getThreadPool().execute(new Runnable() {
		//	public void run() {
		// TODO: use json
		cdvWebView.loadUrl("javascript:cordova.fireDocumentEvent('preferencesChanged',{'key': '" + key + "'})");
		//	}
		//});
	}

	@Override
	public void onResume(boolean multitasking) {
		if (this.watchChanges)
			PreferenceManager.getDefaultSharedPreferences(cordova.getActivity())
			.registerOnSharedPreferenceChangeListener(this);
	}

	@Override
	public void onPause(boolean multitasking) {
		if (this.watchChanges)
			PreferenceManager.getDefaultSharedPreferences(cordova.getActivity())
			.unregisterOnSharedPreferenceChangeListener(this);
	}

	@Override
	public boolean execute(String action, JSONArray args, final CallbackContext callbackContext) throws JSONException {
		//        String result = "";

		if (action.equals ("show")) {
			return this.showPreferencesActivity(callbackContext);
		} else if (action.equals("clearAll")) {
			return this.clearAll(callbackContext);
		} else if (action.equals("watch")) {
			if (args.length() == 1) {
				watchChanges = args.getBoolean(0);
				if (!watchChanges) {
					this.onPause(false);
				} else {
					this.onResume(false);
				}
			} else {
				watchChanges = true;
			}
			callbackContext.success();
			return true;
		}

		JSONObject options = args.getJSONObject (0);
		String key    = options.getString("key");
		String dict   = options.optString("dict");
		String type   = options.optString("type");
		if (!"".equals(dict))
			key = dict + '.' + key;
		// Log.d ("", "key is " + key);


		if (action.equals("fetch")) {
			return this.fetchValueByKey(key, callbackContext);
		} else if (action.equals("store")) {
			String value  = options.getString("value");
			return this.storeValueByKey(key, type, value, callbackContext);
		} else if (action.equals("remove")) {
			return this.removeValueByKey(key, callbackContext);
		}
		// callbackContext.sendPluginResult(new PluginResult (PluginResult.Status.JSON_EXCEPTION));
		return false;
	}

	private boolean clearAll (final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {public void run() {
			SharedPreferences sharedPrefs = PreferenceManager.getDefaultSharedPreferences(cordova.getActivity());

			Editor editor = sharedPrefs.edit();
			editor.clear();
			editor.commit();
			if (editor.commit()) {
				callbackContext.success();
			} else {
				try {
					callbackContext.error(createErrorObj(COMMIT_FAILED, "Cannot commit change"));
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}
		}});
		return true;
	}

	private boolean showPreferencesActivity (final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {public void run() {
			Class preferenceActivity;
			try {
				preferenceActivity = Class.forName("me.apla.cordova.AppPreferencesActivity");
				Intent i = new Intent(cordova.getActivity(), preferenceActivity);
				cordova.getActivity().startActivity(i);
				String result = null;
				callbackContext.success(result);
			} catch(ClassNotFoundException e) {
				callbackContext.error("Class me.apla.cordova.AppPreferencesActivity not found. Please run preference generator.");
				e.printStackTrace();
			} catch (Exception e) {
				callbackContext.error("Intent launch error");
				e.printStackTrace();
			}
		}});
		return true;
	}

	private boolean fetchValueByKey(final String key, final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {public void run() {

			SharedPreferences sharedPrefs = PreferenceManager.getDefaultSharedPreferences(cordova.getActivity());
			String returnVal = null;
			if (sharedPrefs.contains(key)) {
				Object obj = sharedPrefs.getAll().get(key);
				String objClass = obj.getClass().getName();
				if (objClass.equals("java.lang.Integer") || objClass.equals("java.lang.Long")) {
					returnVal = obj.toString();
				} else if (objClass.equals("java.lang.Float") || objClass.equals("java.lang.Double")) {
					returnVal = obj.toString();
				} else if (objClass.equals("java.lang.Boolean")) {
					returnVal = (Boolean)obj ? "true" : "false";
				} else if (objClass.equals("java.lang.String")) {
					if (sharedPrefs.contains("_" + key + "_type")) {
						// here we have json encoded string
						returnVal = (String)obj;
					} else {
						String fakeArray = null;
						try {
							fakeArray = new JSONStringer().array().value((String)obj).endArray().toString();
						} catch (JSONException e) {
							// TODO Auto-generated catch block
							e.printStackTrace();
							callbackContext.error(0);
							return;
						}
						returnVal = fakeArray.substring(1, fakeArray.length()-1);
						// returnVal = new JSONStringer().value((String)obj).toString();
					}

				} else {
					Log.d("", "unhandled type: " + objClass);
				}
				// JSONObject jsonValue = new JSONObject((Map) obj);
				callbackContext.success(returnVal);
			} else {
				// Log.d("", "no value");
				callbackContext.success(returnVal);
				// callbackContext.sendPluginResult(new PluginResult ());
			}

		}});

		return true;
	}

	private boolean removeValueByKey(final String key, final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() { public void run() {

			SharedPreferences sharedPrefs = PreferenceManager.getDefaultSharedPreferences(cordova.getActivity());

			if (sharedPrefs.contains(key)) {
				Editor editor = sharedPrefs.edit();
				editor.remove(key);
				if (sharedPrefs.contains("_" + key + "_type")) {
					editor.remove("_" + key + "_type");
				}

				if (editor.commit()) {
					callbackContext.success();
				} else {
					try {
						callbackContext.error(createErrorObj(COMMIT_FAILED, "Cannot commit change"));
					} catch (JSONException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
				}
			} else {
				callbackContext.sendPluginResult(new PluginResult (PluginResult.Status.NO_RESULT));
			}

		}});

		return true;
	}

	private boolean storeValueByKey(final String key, final String type, final String value, final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {public void run() {

			SharedPreferences sharedPrefs = PreferenceManager.getDefaultSharedPreferences(cordova.getActivity());

			Editor editor = sharedPrefs.edit();
			// editor.putString(key, value);

			Object nv = null;
			try {
				JSONTokener jt = new JSONTokener(value);
				nv = jt.nextValue();
			} catch (NullPointerException e) {
				e.printStackTrace();
			} catch (JSONException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			
			if(nv == null){
				try {
					callbackContext.error(createErrorObj(NULL_VALUE, "Error creating/getting json token"));
					return;
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}

			String className = nv.getClass().getName();

			//      Log.d("", "value is: " + nv.toString() + " js type is: " + type + " " + args.toString());
			if (type != null) {
				if (sharedPrefs.contains("_" + key + "_type")) {
					editor.remove("_" + key + "_type");
				}
				if (type.equals("string") ) {
					editor.putString (key, (String)nv);
				} else if (type.equals("number")) {
					if (className.equals("java.lang.Double")) {
						editor.putFloat(key, ((Double) nv).floatValue());
					} else if (className.equals("java.lang.Integer")) {
						editor.putInt(key, (Integer) nv);
					} else if (className.equals("java.lang.Long")) {
						editor.putLong(key, (Long) nv);
					}
				} else if (type.equals("boolean")) {
					editor.putBoolean (key, (Boolean)nv);
				} else {
					editor.putString(key, value);
					editor.putString ("_" + key + "_type", "json");
					//                Log.d("", "complex thing stored");
				}

			}

			if (editor.commit()) {
				callbackContext.success();
			} else {
				try {
					callbackContext.error(createErrorObj(COMMIT_FAILED, "Cannot commit change"));
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
			}

		}});

		return true;
	}

	private JSONObject createErrorObj(int code, String message) throws JSONException {
		JSONObject errorObj = new JSONObject();
		errorObj.put("code", code);
		errorObj.put("message", message);
		return errorObj;
	}

}
