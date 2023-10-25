# AA1ManualMov

* TEAM DESCRIPTION:
	
	* GRUPO A
	* Arnau Pradas Soriano i Emma Polanco Garcia
	* arnau.pradas@enti.cat / emma.polanco@enti.cat
	* Pictures:

<img width="537" alt="Pictures" src="https://github.com/EmmaENTI/AA1ManualMov/assets/99646207/92a9a284-8b8c-4402-bc41-e55621e98e99">




EXERCICE 2 EXPLANATION:

In my case, I found that using Linear Interpolation (Lerp) was more smooth in my code to transition between joint angles.
I chose it over methods like SLerp because Lerp was simpler to implement and proved more efficient than Slerp in my case.
Also Lerp proved to be more predictable and in constant-speed motion. In my opinion this made the animation look more 
mechanical and controlled.

Overall, I basically chose Lerp because it was easier to implement, understand and honestly I think it looks good... Not much else to say.


I guess on this case you could also use Slerp, because the joint motion of the robot arm is non-linear (joints rotate along curved paths),
Slerp can better represent these complex movements, making it more smooth, but it honestly is much harder to implement at least in my opinion and the 
results are pretty similar. Plus, Slerp could also avoid Gimbal Lock, which can happen if using Lerp with quaternions. 


TO SUMMARIZE: In MY code, Lerp calculates the interpolated values in a linear manner. Another way to implement would be 
to use Spherical Linear Interpolation (SLERP) because it can give more smooth rotations, and is commonly used for smoothly interpolating 3D rotations,
but since when trying and testing out my code LERP worked just fine I simply decided to keep Lerp, not much more to say.


(By the way, I know my README is not using a branch gitflow, but please remember you told me in class that I could simply commit it to main :C )
