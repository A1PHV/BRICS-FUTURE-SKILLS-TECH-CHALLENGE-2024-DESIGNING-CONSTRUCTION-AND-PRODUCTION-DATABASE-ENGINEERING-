

/**
 * Normalized tilt in Hover
 *
 * @min 0.0
 * @max 1.0
 * @increment 0.01
 * @decimal 3
 * @group VTOL Attitude Control
 */
PARAM_DEFINE_FLOAT(VT_TILT_MC, 0.0f);

/**
 * Normalized tilt in transition to FW
 *
 * @min 0.0
 * @max 1.0
 * @increment 0.01
 * @decimal 3
 * @group VTOL Attitude Control
 */
PARAM_DEFINE_FLOAT(VT_TILT_TRANS, 0.4f);

/**
 * Normalized tilt in FW
 *
 * @min 0.0
 * @max 1.0
 * @increment 0.01
 * @decimal 3
 * @group VTOL Attitude Control
 */
PARAM_DEFINE_FLOAT(VT_TILT_FW, 1.0f);

/**
 * Duration of front transition phase 2
 *
 * Time in seconds it takes to tilt form VT_TILT_TRANS to VT_TILT_FW.
 *
 * @unit s
 * @min 0.1
 * @max 5.0
 * @increment 0.01
 * @decimal 3
 * @group VTOL Attitude Control
 */
PARAM_DEFINE_FLOAT(VT_TRANS_P2_DUR, 0.5f);

/**
 * Duration motor tilt up in backtransition
 *
 * Time in seconds it takes to tilt form VT_TILT_FW to VT_TILT_MC.
 *
 * @unit s
 * @min 0.1
 * @max 10
 * @increment 0.1
 * @decimal 1
 * @group VTOL Attitude Control
 */
PARAM_DEFINE_FLOAT(VT_BT_TILT_DUR, 1.f);
