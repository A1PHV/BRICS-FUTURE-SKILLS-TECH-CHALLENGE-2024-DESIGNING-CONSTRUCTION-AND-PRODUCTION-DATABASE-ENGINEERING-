

#ifndef TAILSITTER_H
#define TAILSITTER_H

#include "vtol_type.h"

#include <parameters/param.h>
#include <drivers/drv_hrt.h>
#include <matrix/matrix/math.hpp>

// [rad] Pitch threshold required for completing transition to fixed-wing in automatic transitions
static constexpr float PITCH_THRESHOLD_AUTO_TRANSITION_TO_FW = -1.05f; // -60°

// [rad] Pitch threshold required for completing transition to hover in automatic transitions
static constexpr float PITCH_THRESHOLD_AUTO_TRANSITION_TO_MC = -0.26f; // -15°

// [s] Thrust blending duration from fixed-wing to back transition throttle
static constexpr float B_TRANS_THRUST_BLENDING_DURATION = 0.5f;

class Tailsitter : public VtolType
{

public:
	Tailsitter(VtolAttitudeControl *_att_controller);
	~Tailsitter() override = default;

	void update_vtol_state() override;
	void update_transition_state() override;
	void update_fw_state() override;
	void fill_actuator_outputs() override;
	void waiting_on_tecs() override;
	void blendThrottleAfterFrontTransition(float scale) override;
	void blendThrottleBeginningBackTransition(float scale);

private:
	enum class vtol_mode {
		MC_MODE = 0,			/**< vtol is in multicopter mode */
		TRANSITION_FRONT_P1,	/**< vtol is in front transition part 1 mode */
		TRANSITION_BACK,		/**< vtol is in back transition mode */
		FW_MODE					/**< vtol is in fixed wing mode */
	};

	vtol_mode _vtol_mode{vtol_mode::MC_MODE};			/**< vtol flight mode, defined by enum vtol_mode */

	bool _flag_was_in_trans_mode = false;	// true if mode has just switched to transition

	matrix::Quatf _q_trans_start;
	matrix::Quatf _q_trans_sp;
	matrix::Vector3f _trans_rot_axis;

	void parameters_update() override;

	bool isFrontTransitionCompletedBase() override;

	DEFINE_PARAMETERS_CUSTOM_PARENT(VtolType,
					(ParamFloat<px4::params::FW_PSP_OFF>) _param_fw_psp_off
				       )


};
#endif
