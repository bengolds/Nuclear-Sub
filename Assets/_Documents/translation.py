import random
chars = ['A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z']

while len(chars) > 0:
	char1 = chars[random.randint(0, len(chars)-1)]
	chars.remove(char1)
	char2 = chars[random.randint(0, len(chars)-1)]
	chars.remove(char2)
	print(char1 + ":" + char2)
	