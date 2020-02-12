using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestUtils {
    /// <summary>
    /// Warning to users that the method you're using is intended for testing purposes
    /// only, so do not use it directly. Behaviour is undefined if you do.
    /// </summary>
    public static void logTestingWarning() {
        Debug.LogWarning("This method is not intended to be used inside of your actual game.");
    }
}
