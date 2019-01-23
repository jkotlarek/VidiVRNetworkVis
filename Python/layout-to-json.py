#Author: Joseph Kotlarek
#Last Updated: 1/22/2019

#Usage:
#>python layout-to-json.py --files <file1> <file2> ...

#note: do not include .layout extension in filename, it is assumed.
#note: output file is named '<fileX>.json' in same directory as input file.

import os
import json
import argparse

parser = argparse.ArgumentParser(description='Convert .layout files to .json')
parser.add_argument('--files', dest='files', type=str, nargs='+', help='names of files to convert, not including extensions.')

args = parser.parse_args()


for filename in args.files:
	print(filename)
	edgelist = []
	nodelist = []
	nodelookup = []
	with open(filename + '.layout', 'r') as layoutfile:
		while True:
			line = layoutfile.readline()

			if (line == ''):
				break

			if (line.startswith('//NODECOORD')):
				#Add node
				n = line.split()
				s = line.split('"')
				nodelookup.append(s[1])
				nodelist.append({'label':s[1], 'x':n[2], 'y':n[3], 'z':n[4]})

			elif (line.startswith('"')):
				#Add edge
				s = line.split('"')
				edgelist.append({'source':s[1], 'target':s[3], 'value':1})

	for edge in edgelist:
		edge['source'] = nodelookup.index(edge['source'])
		edge['target'] = nodelookup.index(edge['target'])

	jsondata = {'nodes':nodelist, 'links':edgelist}

	open(filename + '.json', 'w').write(json.dumps(jsondata, indent=4, separators=(',',': ')))