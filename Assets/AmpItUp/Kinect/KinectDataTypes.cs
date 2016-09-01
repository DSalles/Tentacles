using System;
namespace KinectLink {

	public enum StreamState {
		Idle,
		Streaming,
		Recording
	}

	public enum TrackingState {
		// Summary:
		//     The joint data is not tracked and no data is known about this joint.
		NotTracked = 0,
		//
		// Summary:
		//     The joint data is inferred and confidence in the position data is lower than
		//     if it were Tracked.
		Inferred = 1,
		//
		// Summary:
		//     The joint data is being tracked and the data can be trusted.
		Tracked = 2,
	}

	// Summary:
	//     The state of a hand of a body.
	public enum HandState {
		// Summary:
		//     Undetermined hand state.
		Unknown = 0,
		//
		// Summary:
		//     Hand not tracked.
		NotTracked = 1,
		//
		// Summary:
		//     Open hand.
		Open = 2,
		//
		// Summary:
		//     Closed hand.
		Closed = 3,
		//
		// Summary:
		//     Lasso (pointer) hand.
		Lasso = 4,
	}

	// Summary:
	//     Specifies the confidence level of a body's tracked attribute.
	public enum TrackingConfidence {
		// Summary:
		//     Low confidence.
		Low = 0,
		//
		// Summary:
		//     High confidence.
		High = 1,
	}
}

