import networkx as nx
import xml.etree.ElementTree as ET

graph = nx.Graph()

tree = ET.parse("terr.xml")
nodes = tree.find('nodelist').findall('node')
edges = tree.find('edgelist').findall('edge')

for n in nodes:
	graph.add_node(n.get('id'))

for e in edges:
	graph.add_edge(e.get('source'), e.get('destination'))

nx.write_gml(graph, "terr.gml")