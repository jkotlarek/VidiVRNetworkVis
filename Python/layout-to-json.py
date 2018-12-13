import os
import json

edgelist = []
nodelist = []
nodelookup = []

with open('terr.layout', 'r') as layoutfile:
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

open('terr.json', 'w').write(json.dumps(jsondata, indent=4, separators=(',',': ')))