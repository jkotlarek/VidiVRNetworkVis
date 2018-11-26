import argparse
from xml.etree.ElementTree import ElementTree
import json
import os

indir = "./"
outdir = "../Assets/StreamingAssets/3D/"


#This function converts a .graphml file to .json
def convert(filename):
	file = open(filename, 'r')

	tree = ElementTree()
	tree.parse(file);

	file.close()

	graphml = {
		"graph": "{http://graphml.graphdrawing.org/xmlns}graph",
		"node": "{http://graphml.graphdrawing.org/xmlns}node",
		"edge": "{http://graphml.graphdrawing.org/xmlns}edge",
		"data": "{http://graphml.graphdrawing.org/xmlns}data",
		"label": "{http://graphml.graphdrawing.org/xmlns}data[@key='label']",
		"x": "{http://graphml.graphdrawing.org/xmlns}data[@key='x']",
		"y": "{http://graphml.graphdrawing.org/xmlns}data[@key='y']",
		"z": "{http://graphml.graphdrawing.org/xmlns}data[@key='z']",
		"size": "{http://graphml.graphdrawing.org/xmlns}data[@key='size']",
		"r": "{http://graphml.graphdrawing.org/xmlns}data[@key='r']",
		"g": "{http://graphml.graphdrawing.org/xmlns}data[@key='g']",
		"b": "{http://graphml.graphdrawing.org/xmlns}data[@key='b']",
		"weight": "{http://graphml.graphdrawing.org/xmlns}data[@key='weight']",
		"edgeid": "{http://graphml.graphdrawing.org/xmlns}data[@key='edgeid']"
	}

	graph = tree.find(graphml.get("graph"))
	nodes = graph.findall(graphml.get("node"))
	edges = graph.findall(graphml.get("edge"))

	out = {"nodes":[], "links":[]}

	for node in nodes[:]:
		out["nodes"].append({
			"id": node.get("id"),
			"x": float(getattr(node.find(graphml.get("x")), "text", 0)),
			"y": float(getattr(node.find(graphml.get("y")), "text", 0)),
			"z": float(getattr(node.find(graphml.get("z")), "text", 0)),
			"size": float(getattr(node.find(graphml.get("size")), "text", 0))
		})

	for edge in edges[:]:
		out["links"].append({
			"id": edge.get("id"),
			"source": edge.get("source"),
			"target": edge.get("target")
		})


	#fix misnumbered nodes/edges
	#this only works because IDs are still sorted in ascending order
	i = 0;
	for node in out['nodes']:
		
		old_id = node['id']
		node['id'] = i;
		
		j = 0
		for link in out['links']:
			
			if link['source'] == old_id:
				link['source'] = i
			
			if link['target'] == old_id:
				link['target'] = i

			link['id'] = j
			j += 1

		i += 1



	return json.dumps(out)


#Script:

files = list(filter(lambda s: s[len(s)-8:] == ".graphml", os.listdir(indir)))

for f in files:
	#Do the convert
	json_out = convert(indir + f)
	#Save to outdir
	outfilename = f[:len(f)-len(".graphml")] + ".json"
	open(outdir + outfilename, 'w').write(json_out)
	