#!/usr/bin/python

import sys
import csv
import os
import re
import shutil

data_filename = sys.argv[1];
html_filename = sys.argv[2];
images_directory = sys.argv[3] if len(sys.argv) >= 4 else 'Resources'
with open(data_filename, 'rb') as csvfile:
	with open (html_filename, 'r') as htmlfile:
		html = htmlfile.read()
		data_reader = csv.reader(csvfile)
		template_values = data_reader.next()

		temp_directory = 'temp'
		if not os.path.exists(temp_directory):
			os.makedirs(temp_directory)
		if not os.path.exists(images_directory):
			os.makedirs(images_directory)

		for row in data_reader:
			subbed = html
			i = 0
			for subvalue in template_values:
				subbed = re.sub(subvalue, row[i], subbed)
				i = i+1
			outfilename = row[0] + ".html"
			with open (outfilename, 'w') as outfile:
				outfile.write(subbed)
			outimagename = images_directory + "/" + row[0] + ".png"

			cmd = "./wkhtmltoimage --width 816 --height 1056 --disable-smart-width  " + outfilename + " " + outimagename
			print(cmd)
			os.system(cmd)
			# os.remove(outfilename)

