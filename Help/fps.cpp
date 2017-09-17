//fixed fps
const int FRAMES_PER_SECOND = 25;
const int SKIP_TICKS = 1000 / FRAMES_PER_SECOND;

DWORD next_game_tick = GetTickCount();
// GetTickCount() returns the current number of milliseconds
// that have elapsed since the system was started

int sleep_time = 0;

bool game_is_running = true;

while( game_is_running ) {
	update_game();
	display_game();

	next_game_tick += SKIP_TICKS;
	sleep_time = next_game_tick - GetTickCount();
	if( sleep_time >= 0 ) {
		Sleep( sleep_time );
	}
	else {
		// Shit, we are running behind!
	}
}
	
	
//variable fps, depending on the speed of drawing the previous frame
DWORD prev_frame_tick;
DWORD curr_frame_tick = GetTickCount();

bool game_is_running = true;
while( game_is_running ) {
	prev_frame_tick = curr_frame_tick;
	curr_frame_tick = GetTickCount();

	update_game( curr_frame_tick - prev_frame_tick );
	display_game();
}