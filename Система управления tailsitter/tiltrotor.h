

#ifndef TILTROTOR_H
#define TILTROTOR_H
#include "vtol_type.h"
#include <parameters/param.h>
#include <drivers/drv_hrt.h>

#include <uORB/Publication.hpp>
#include <uORB/topics/tiltrotor_extra_controls.h>

class Tiltrotor : public VtolType
{

public:

	Tiltrotor(VtolAttitudeControl *_att_controller);
	~Tiltrotor() override = default;

	void update_vtol_state() override;
	void update_transition_state() override;
	void fill_actuator_outputs() override;
	void update_mc_state() override;
	void update_fw_state() override;
	void waiting_on_tecs() override;
	void blendThrottleAfterFrontTransition(float scale) override;

private:
	enum class vtol_mode {
		MC_MODE = 0,			/**< vtol is in multicopter mode */
		TRANSITION_FRONT_P1,	/**< vtol is in front transition part 1 mode */
		TRANSITION_FRONT_P2,	/**< vtol is in front transition part 2 mode */
		TRANSITION_BACK,		/**< vtol is in back transition mode */
		FW_MODE					/**< vtol is in fixed wing mode */
	};

	/**
	 * Specific to tiltrotor with vertical aligned rear engine/s.
	 * These engines need to be shut down in fw mode. During the back-transition
	 * they need to idle otherwise they need too much time to spin up for mc mode.
	 */

	vtol_mode _vtol_mode{vtol_mode::MC_MODE};			/**< vtol flight mode, defined by enum vtol_mode */

	uORB::Publication<tiltrotor_extra_controls_s>	_tiltrotor_extra_controls_pub{ORB_ID(tiltrotor_extra_controls)};

	float _tilt_control{0.0f};		/**< actuator value for the tilt servo */

	void parameters_update() override;
	float timeUntilMotorsAreUp();
	float moveLinear(float start, float stop, float progress);

	void blendThrottleDuringBacktransition(const float scale, const float target_throttle);
	bool isFrontTransitionCompletedBase() override;


	DEFINE_PARAMETERS_CUSTOM_PARENT(VtolType,
					(ParamFloat<px4::params::VT_TILT_MC>) _param_vt_tilt_mc,
					(ParamFloat<px4::params::VT_TILT_TRANS>) _param_vt_tilt_trans,
					(ParamFloat<px4::params::VT_TILT_FW>) _param_vt_tilt_fw,
					(ParamFloat<px4::params::VT_TRANS_P2_DUR>) _param_vt_trans_p2_dur,
					(ParamFloat<px4::params::VT_BT_TILT_DUR>) _param_vt_bt_tilt_dur
				       )

};
#endif
