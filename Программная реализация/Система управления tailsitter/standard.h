#ifndef STANDARD_H
#define STANDARD_H
#include "vtol_type.h"

class Standard : public VtolType
{

public:

	Standard(VtolAttitudeControl *_att_controller);
	~Standard() override = default;

	void update_vtol_state() override;
	void update_transition_state() override;
	void update_fw_state() override;
	void update_mc_state() override;
	void fill_actuator_outputs() override;
	void waiting_on_tecs() override;
	void blendThrottleAfterFrontTransition(float scale) override;

private:

	enum class vtol_mode {
		MC_MODE = 0,
		TRANSITION_TO_FW,
		TRANSITION_TO_MC,
		FW_MODE
	};

	vtol_mode _vtol_mode{vtol_mode::MC_MODE};			/**< vtol flight mode, defined by enum vtol_mode */

	float _pusher_throttle{0.0f};
	float _airspeed_trans_blend_margin{0.0f};
	hrt_abstime _last_time_pusher_transition_update{0};

	void parameters_update() override;

	DEFINE_PARAMETERS_CUSTOM_PARENT(VtolType,
					(ParamFloat<px4::params::VT_PSHER_SLEW>) _param_vt_psher_slew,
					(ParamFloat<px4::params::VT_B_TRANS_RAMP>) _param_vt_b_trans_ramp,
					(ParamFloat<px4::params::FW_PSP_OFF>) _param_fw_psp_off
				       )
};
#endif
