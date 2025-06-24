import os

def to_pascal_case(snake_case_string: str) -> str:
    """Converts a snake_case string to PascalCase."""
    return "".join(word.capitalize() for word in snake_case_string.split('_'))

def find_unadapted_files():
    """
    Finds XML files in the example directory that do not have a corresponding
    C# model file in the project.
    """
    xml_dir = "example/ModuleData"
    models_root_dir = "BannerlordModEditor.Common/Models"
    
    # Gather all subdirectories in the models folder
    model_search_dirs = [models_root_dir]
    for root, dirs, _ in os.walk(models_root_dir):
        for d in dirs:
            if d not in ["bin", "obj"]:
                model_search_dirs.append(os.path.join(root, d))

    print(f"Checking for unadapted XML files in '{xml_dir}'...")
    print(f"Searching for models in: {', '.join(model_search_dirs)}")
    print("--------------------------------------------------")

    try:
        xml_files = [f for f in os.listdir(xml_dir) if f.endswith('.xml') and os.path.isfile(os.path.join(xml_dir, f))]
    except FileNotFoundError:
        print(f"Error: Directory '{xml_dir}' not found.")
        return

    unadapted_files = []

    for xml_file in xml_files:
        base_name = os.path.splitext(xml_file)[0]
        pascal_case_name = to_pascal_case(base_name)
        
        expected_model_file = f"{pascal_case_name}.cs"
        
        found = False
        for search_dir in model_search_dirs:
            # Standard check
            model_path = os.path.join(search_dir, expected_model_file)
            if os.path.exists(model_path):
                found = True
                break
            
            # Also check for plural form, e.g. mpitem.xml -> MpItems.cs
            plural_model_path = os.path.join(search_dir, f"{pascal_case_name}s.cs")
            if os.path.exists(plural_model_path):
                found = True
                break

        if not found:
            unadapted_files.append(xml_file)

    if unadapted_files:
        print(f"Found {len(unadapted_files)} potentially unadapted XML files:")
        # Sort files for consistent output
        for f in sorted(unadapted_files):
            print(f"- {f}")
    else:
        print("All XML files seem to be adapted based on the file naming convention.")

if __name__ == "__main__":
    find_unadapted_files() 