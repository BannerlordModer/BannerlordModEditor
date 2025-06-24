import os
import xml.etree.ElementTree as ET

def split_xml(file_path, output_dir, element_tag, chunk_size=500):
    """
    Splits a large XML file into smaller chunks.

    Args:
        file_path (str): Path to the large XML file.
        output_dir (str): Directory to save the smaller XML files.
        element_tag (str): The tag of the elements to split by.
        chunk_size (int): Number of elements per chunk.
    """
    if not os.path.exists(output_dir):
        os.makedirs(output_dir)

    try:
        tree = ET.parse(file_path)
        root = tree.getroot()
    except ET.ParseError as e:
        print(f"Error parsing XML file: {e}")
        return
    except FileNotFoundError:
        print(f"Error: File not found at {file_path}")
        return

    elements = root.findall(element_tag)
    total_elements = len(elements)
    
    print(f"Found {total_elements} <{element_tag}> elements in '{file_path}'.")
    
    # Preserve root attributes
    root_attributes = root.attrib

    for i in range(0, total_elements, chunk_size):
        chunk = elements[i:i + chunk_size]
        
        # Create a new root for the chunk
        new_root = ET.Element(root.tag, attrib=root_attributes)
        new_root.extend(chunk)
        
        # Create a new tree for the chunk
        new_tree = ET.ElementTree(new_root)
        
        # Format the output file name
        base_name = os.path.splitext(os.path.basename(file_path))[0]
        output_filename = os.path.join(output_dir, f"{base_name}_part_{i // chunk_size + 1}.xml")
        
        # Write to file with XML declaration
        with open(output_filename, 'wb') as f:
            f.write(b'<?xml version="1.0" encoding="utf-8"?>\n')
            new_tree.write(f, encoding='utf-8')
        
        print(f"Created chunk: {output_filename}")

if __name__ == '__main__':
    # Configuration for action_types.xml
    INPUT_FILE = "example/ModuleData/action_types.xml"
    OUTPUT_DIRECTORY = "BannerlordModEditor.Common.Tests/TestSubsets/ActionTypes"
    ELEMENT_TAG_TO_SPLIT = "action"
    CHUNK_SIZE = 500
    
    split_xml(INPUT_FILE, OUTPUT_DIRECTORY, ELEMENT_TAG_TO_SPLIT, CHUNK_SIZE)
    print("\nSplitting complete.") 