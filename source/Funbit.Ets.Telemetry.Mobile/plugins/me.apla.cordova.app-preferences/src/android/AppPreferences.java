package me.apla.cordova;

//import java.util.Iterator;
//import java.util.Map;

import org.apache.cordova.CallbackContext;
import org.apache.cordova.CordovaPlugin;
import org.apache.cordova.PluginResult;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONStringer;
import org.json.JSONTokener;

//import android.content.ActivityNotFoundException;
//import android.content.Intent;
import android.content.SharedPreferences;
import android.content.SharedPreferences.Editor;
import android.preference.PreferenceManager;
import android.util.Log;

// http://developer.android.com/guide/topics/ui/settings.html
// http://stackoverflow.com/questions/4990529/android-a-good-looking-standard-settings-menu
// http://androidpartaker.wordpress.com/2010/07/11/android-preferences/

/*

<?xml version="1.0" encoding="utf-8"?>
<PreferenceScreen xmlns:android="http://schemas.android.com/apk/res/android">
	<PreferenceCategory android:title="Main">
		<CheckBoxPreference android:title="Enable Preferences"
			android:key="EnablePreferences" android:summary="Check to enable Other Preferences" />
	</PreferenceCategory>
	<PreferenceCategory android:title="Other Prefernces">
		<ListPreference android:title="List Preference"
			android:key="DayOfWeek" android:dependency="EnablePreferences"
			android:summary="Selec Day of the Week" android:entries="@array/daysOfWeek"
			android:entryValues="@array/daysOfWeekValues" />
		<EditTextPreference android:title="Edit Text Preference"
			android:key="Name" android:dependency="EnablePreferences"
			android:summary="Enter Your Name" android:dialogTitle="Enter Your Name"
			android:defaultValue="Android Partaker"/>
		<RingtonePreference android:title="Ringtone Preference"
			android:key="Ringtone" android:dependency="EnablePreferences"
			android:summary="Select Ringtone" android:ringtoneType="all" />
	</PreferenceCategory>

	<PreferenceCategory android:title="Advance Preference">
		<PreferenceScreen android:title="Advance Preference">

			<EditTextPreference android:title="Enter Text"
				android:key="Text" />
		</PreferenceScreen>
	</PreferenceCategory>
</PreferenceScreen>

*/

public class AppPreferences extends CordovaPlugin {

//    private static final String LOG_TAG = "AppPreferences";
//    private static final int NO_PROPERTY = 0;
//    private static final int NO_PREFERENCE_ACTIVITY = 1;
	private static final int COMMIT_FAILED = 2;


	@Override
	public boolean execute(String action, JSONArray args, final CallbackContext callbackContext) throws JSONException {
//        String result = "";


		if (action.equals("fetch")) {
			JSONObject options = args.getJSONObject (0);
			String key  = options.getString("key");
			String dict = options.optString("dict");
			if (!"".equals(dict))
				key = dict + '.' + key;
			Log.d ("", "key is " + key);
			return this.fetchValueByKey(key, callbackContext);
		} else if (action.equals("store")) {
			JSONObject options = args.getJSONObject (0);
			String key    = options.getString("key");
			String value  = options.getString("value");
			String dict   = options.optString("dict");
			String type   = options.optString("type");
			if (!"".equals(dict))
				key = dict + '.' + key;
			Log.d ("", "key is " + key);
			return this.storeValueByKey(key, type, value, callbackContext);
//            } else if (action.equals("load")) {
//                JSONObject obj = new JSONObject();
//                Map prefs = sharedPrefs.getAll();
//                Iterator it = prefs.entrySet().iterator();
//                while (it.hasNext()) {
//                    Map.Entry pairs = (Map.Entry)it.next();
//                    obj.put(pairs.getKey().toString(), pairs.getValue().toString());
//                }
//                callbackContext.sendPluginResult(new PluginResult(status, obj));
//            } else if (action.equals("show")) {
//                String activityName = args.getString(0);
//                Intent intent = new Intent(Intent.ACTION_VIEW);
//                intent.setClassName(this.cordova.getActivity(), activityName);
//                try {
//                    this.cordova.getActivity().startActivity(intent);
//                } catch (ActivityNotFoundException e) {
//                    callbackContext.sendPluginResult(createErrorObj(NO_PREFERENCE_ACTIVITY, "No preferences activity called " + activityName));
//                }
		} else if (action.equals("removeAll")) {
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
		} else if (action.equals("remove")) {
			JSONObject options = args.getJSONObject (0);
			String key  = options.getString("key");
			String dict = options.getString("dict");
			if (!"".equals(dict))
					key = dict + '.' + key;
			removeValueByKey(key, callbackContext);
		}
		// callbackContext.sendPluginResult(new PluginResult (PluginResult.Status.JSON_EXCEPTION));
		return false;
	}

	private boolean fetchValueByKey(final String key, final CallbackContext callbackContext) {
		cordova.getThreadPool().execute(new Runnable() {public void run() {

		SharedPreferences sharedPrefs = PreferenceManager.getDefaultSharedPreferences(cordova.getActivity());
		String returnVal = null;
		if (sharedPrefs.contains(key)) {
			Object obj = sharedPrefs.getAll().get(key);
			String objClass = obj.getClass().getName();
			if (objClass.equals("java.lang.Integer")) {
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
//          Log.d("", "no value");
				callbackContext.error(0);
//            callbackContext.sendPluginResult(new PluginResult ());
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

		JSONTokener jt = new JSONTokener(value);
		Object nv = null;
		try {
			nv = jt.nextValue();
		} catch (JSONException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
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
