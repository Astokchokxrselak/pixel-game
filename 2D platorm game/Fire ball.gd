extends KinematicBody2D
const bulletpath = preload("res://Fire ball.tscn")
const UP = Vector2(0,-1)
const GRAVITY = 15
const MAXFALLSPEED = 1000
const MAXSPEED = 600
const JUMPFORCE = 800


var motion = Vector2()

func _ready():
	pass 


func shoot():
	var bullet = bulletpath.instance()
	get_parent().add_child(bullet)
	bullet.position = $Position2D.global_position


func _physics_process(delta):
	
	
	
	motion.y += GRAVITY
	if motion.y >MAXFALLSPEED:
		motion.y =MAXFALLSPEED
	motion.x = 0
	if Input.is_action_pressed("right"):
	 motion.x = MAXSPEED
	elif Input.is_action_pressed("left"):
		motion.x = -MAXSPEED
	else:
		motion.x = 0
		
	if is_on_floor():
		if Input.is_action_just_pressed("jump"):
			motion.y = -JUMPFORCE
	motion = move_and_slide(motion,UP)

func _process(delta):
	if Input.is_action_just_pressed("ui_accept"):
		print("I'm shooting")
		shoot()
		
		
